import { Injectable } from "@angular/core";
import { action, makeObservable, observable, runInAction } from "mobx";
import { FileStorageService } from "../../services/file-storage.service";
import { UserService } from "../../services/user.service";
import { IUser } from "../../models/User/IUser";
import { map, mergeMap, shareReplay, tap } from "rxjs/operators";
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

  public updateUserUrl(userId: number, imageId: string): void {
    const user = this.getEntity(userId);
    user.profileImageId = imageId;
    this.fileStorageService.getById(user.profileImageId)
      .pipe(tap((safeUrl) => this.updateUserImage([safeUrl, user])));
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
              of(user);
          }),
          map((res: IUser | [SafeUrl, IUser]) => this.updateUserImage(res)),
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

  private updateUserImage(res: IUser | [SafeUrl, IUser]): IUser {
    if (Array.isArray(res)) {
      res[1].image = res[0];
      this.addEntity(res[1]);
      return res[1];
    } else {
      return res;
    }
  }

  private static setUserInitials(user: IUser): string {
    return user.userName?.split(' ')
        .reduce((x,y) => x.concat(y))
        .substring(0,2)
        .toUpperCase()
      ?? '';
  }
}
