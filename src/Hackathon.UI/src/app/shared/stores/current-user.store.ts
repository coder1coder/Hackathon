import { DictionariesLoading } from '../../common/base-components/dictionaries-loading.component';
import { Injectable, inject } from '@angular/core';
import { IUser } from '../../models/User/IUser';
import { AuthService } from '../../services/auth.service';
import { action, makeObservable, observable, runInAction } from 'mobx';

@Injectable({
  providedIn: 'root',
})
export class CurrentUserStore extends DictionariesLoading {
  private authService = inject(AuthService);

  @observable currentUser: IUser | null = null;

  constructor() {
    super();
    makeObservable(this);
  }

  @action
  public loadCurrentUser(needReload: boolean = false): void {
    if (needReload || (!this.isDictLoading('loadCurrentUser') && !this.currentUser)) {
      this.changeLoadingStatus('loadCurrentUser');
      this.authService.getCurrentUser().subscribe((res) => {
        runInAction(() => {
          this.currentUser = res;
          this.changeLoadingStatus('loadCurrentUser');
        });
      });
    }
  }

  @action
  public clearStore(): void {
    runInAction(() => {
      this.currentUser = null;
    });
  }
}
