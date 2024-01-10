import { Injectable } from '@angular/core';
import { action, makeObservable, observable, runInAction } from 'mobx';
import { FileStorageService } from '../../services/file-storage.service';
import { UserService } from '../../services/user.service';
import { IUser } from '../../models/User/IUser';
import { map, shareReplay, switchMap } from 'rxjs/operators';
import { catchError, forkJoin, Observable, of, throwError } from 'rxjs';
import { SafeUrl } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root',
})
export class ProfileUserStore {
  @observable protected users$: Map<number, IUser> = new Map<number, IUser>();
  private fetchObservableMap: Map<number, Observable<IUser>> = new Map<number, Observable<IUser>>();

  constructor(private fileStorageService: FileStorageService, private userService: UserService) {
    makeObservable(this);
  }

  public getUser(userId: number, needUpdate = false): Observable<IUser> {
    if (needUpdate) {
      return this.fetchUser(userId);
    }

    if (this.users$.has(userId)) {
      return of(this.getEntity(userId));
    } else {
      return this.fetchUser(userId);
    }
  }

  public updateUserUrl(user: IUser, imageId: string): Observable<IUser> {
    const existUser: IUser = this.getEntity(user.id);
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
      const request: Observable<IUser> = this.userService.getById(userId).pipe(
        shareReplay(1),
        switchMap((user: IUser) => {
          user.shortUserName = ProfileUserStore.setUserInitials(user);
          user.bgColor = this.generateColorFromName(user.fullName ?? user.userName);
          this.addEntity(user);
          return user?.profileImageId
            ? forkJoin([
                this.fileStorageService
                  .getById(user.profileImageId)
                  .pipe(catchError(() => of(null))),
                of(user),
              ])
            : forkJoin([of(null), of(user)]);
        }),
        map((res: [SafeUrl, IUser]) => this.updateUserImage(res)),
        catchError(() => throwError(() => new Error('Пользователь не найден'))),
      );
      this.fetchObservableMap.set(userId, request);
      return request;
    }
  }

  @action
  private addEntity(user: IUser): void {
    runInAction(() => {
      this.users$.set(user.id, user);
    });
  }

  private getEntity(userId: number): IUser {
    return this.users$.get(userId);
  }

  private loadImageUpdateUser(user: IUser, imageId: string): Observable<IUser> {
    if (!user.shortUserName) {
      user.shortUserName = ProfileUserStore.setUserInitials(user);
    }
    if (!user.bgColor) {
      user.bgColor = this.generateColorFromName(user.fullName ?? user.userName);
    }
    if (!imageId) {
      user.profileImageId = null;
      return of(this.updateUserImage([null, user]));
    }
    user.profileImageId = imageId ?? null;
    return user.profileImageId
      ? this.fileStorageService
          .getById(user.profileImageId)
          .pipe(map((safeUrl) => this.updateUserImage([safeUrl, user])))
      : of(this.updateUserImage([null, user]));
  }

  private updateUserImage(res: [SafeUrl, IUser]): IUser {
    const user: IUser = res[1];
    user.image = res[0];
    this.addEntity(user);
    this.fetchObservableMap.delete(user.id);
    return user;
  }

  private static setUserInitials(user: IUser): string {
    return (
      user.userName
        ?.split(' ')
        .reduce((x, y) => x.concat(y))
        .substring(0, 2)
        .toUpperCase() ?? ''
    );
  }

  private readonly colors: string[] = [
    '#50723C',
    '#7C616C',
    '#005377',
    '#E2DE84',
    '#ab2cb9',
    '#8B9474',
    '#4a8554',
    '#9f4e00',
    '#A4778B',
    '#F5A65B',
  ];

  private generateColorFromName(name: string): string {
    let hash: number = 0;
    for (let i: number = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }

    const baseRGB: number = parseInt(this.generateColorForUser(name).slice(1), 16);
    const baseRed: number = (baseRGB >> 16) & 255;
    const baseGreen: number = (baseRGB >> 8) & 255;
    const baseBlue: number = baseRGB & 255;
    const brightnessOffset: number = (hash % 35) - 15;
    const tonedRed: number = Math.max(0, Math.min(255, baseRed + brightnessOffset));
    const tonedGreen: number = Math.max(0, Math.min(255, baseGreen + brightnessOffset));
    const tonedBlue: number = Math.max(0, Math.min(255, baseBlue + brightnessOffset));

    return `#${((1 << 24) | (tonedRed << 16) | (tonedGreen << 8) | tonedBlue)
      .toString(16)
      .slice(1)}`;
  }

  private generateColorForUser(userName: string): string {
    let hash: number = 0;
    for (let i: number = 0; i < userName.length; i++) {
      hash = userName.charCodeAt(i) + ((hash << 5) - hash);
    }
    const index: number = Math.abs(hash) % this.colors.length;
    return this.colors[index];
  }
}
