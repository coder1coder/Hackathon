import { Injectable } from "@angular/core";
import { action, makeObservable, observable, runInAction } from "mobx";
import { FileStorageService } from "../../services/file-storage.service";
import { UserService } from "../../services/user.service";
import { IUser } from "../../models/User/IUser";
import { map, mergeMap, shareReplay } from "rxjs/operators";
import { catchError, forkJoin, Observable, of, throwError } from "rxjs";
import { fromMobx } from "../../common/functions/from-mobx.function";
import { SafeUrl } from "@angular/platform-browser";

@Injectable({
  providedIn:'root'
})
export class ProfileUserStore {
  @observable protected users$: Map<number, IUser> = new Map<number, IUser>();
  private fetchObservableMap: Map<number, Observable<IUser>> = new Map<number, Observable<IUser>>();

  constructor(
    private fileStorageService: FileStorageService,
    private userService: UserService,
  ) {
    makeObservable(this);
  }

  public getUser(userId: number, needUpdate = false): Observable<IUser> {
    if (needUpdate) {
      return this.fetchUser(userId);
    }

    if (this.users$.has(userId)) {
      return fromMobx(() => this.getEntity(userId));
    } else {
      return this.fetchUser(userId);
    }
  }

  public updateUserUrl(user: IUser, imageId: string): Observable<IUser> {
    const existUser = this.getEntity(user.id);
    if (existUser) {
      return this.loadImageUpdateUser(existUser, imageId);
    } else {
      return this.loadImageUpdateUser(user, imageId);
    }
  }

  @action
  public clearStore(): void {
    runInAction(() => {
      this.users$.clear();
      this.fetchObservableMap.clear();
    });
  }

  private fetchUser(userId: number): Observable<IUser> {
    if (this.fetchObservableMap.has(userId)) {
      return this.fetchObservableMap.get(userId);
    } else {
      const request = this.userService.getById(userId)
        .pipe(
          shareReplay(1),
          mergeMap((user: IUser) => {
            user.shortUserName = ProfileUserStore.setUserInitials(user);
            this.fetchObservableMap.delete(userId);
            this.addEntity(user);
            return user?.profileImageId ?
              forkJoin([this.fileStorageService.getById(user.profileImageId)
                .pipe(catchError(() => of(null))),  of(user)]) :
              forkJoin([of(null), of(user)]);
          }),
          map((res: [SafeUrl, IUser]) => this.updateUserImage(res)),
          catchError(() => throwError(() => new Error('Пользователь не найден'))),
        )
      this.fetchObservableMap.set(userId, request);
      return request;
    }
  }

  @action
  private addEntity(user: IUser): void {
    runInAction(() => {
      this.users$.set(user.id, user);
    })
  }

  private getEntity(userId: number): IUser {
    return this.users$.get(userId);
  }

  private loadImageUpdateUser(user: IUser, imageId: string): Observable<IUser> {
    if (!imageId) {
      user.shortUserName = ProfileUserStore.setUserInitials(user);
      user.profileImageId = null;
      return of(this.updateUserImage([null, user]));
    }
    user.profileImageId = imageId;
    return this.fileStorageService.getById(user.profileImageId)
      .pipe(map((safeUrl) => this.updateUserImage([safeUrl, user])));
  }

  private updateUserImage(res: [SafeUrl, IUser]): IUser {
    res[1].image = res[0];
    this.addEntity(res[1]);
    return res[1];
  }

  private static setUserInitials(user: IUser): string {
    return user.userName?.split(' ')
      .reduce((x,y) => x.concat(y))
      .substring(0,2)
      .toUpperCase() ?? '';
  }
}
