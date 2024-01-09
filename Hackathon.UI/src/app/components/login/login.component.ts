import '@angular/compiler';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from "../../services/auth.service";
import { finalize } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { GoogleUser } from 'src/app/models/User/GoogleUser';
import { FormBuilder } from '@angular/forms';
import { RouterService } from "../../services/router.service";
import { SnackService } from "../../services/snack.service";
import { ErrorProcessorService } from "../../services/error-processor.service";
import { Observable, Subject, takeUntil } from "rxjs";
import { IProblemDetails } from "../../models/IProblemDetails";
import { fromMobx} from "../../common/functions/from-mobx.function";
import { AppStateService } from "../../services/app-state.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements AfterViewInit  {

  @ViewChild('login', { static: true }) inputLogin: ElementRef;

  public welcomeText: string = 'Добро пожаловать в систему Hackathon';
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);
  public isPassFieldHide: boolean = true;
  public siteKey: string;
  public captchaEnabled: boolean = environment.captchaEnabled;
  public googleClientEnabled: boolean = true;
  public profileForm = this.fb.group({
    login: [null],
    password: [null],
  });

  private captcha: string = "";
  private destroy$ = new Subject();

  constructor (
    private router: Router,
    private routerService: RouterService,
    private errorProcessor: ErrorProcessorService,
    private snackService: SnackService,
    private authService: AuthService,
    private fb: FormBuilder,
    private appStateService: AppStateService
  ) {
    if (router.url === '/logout') {
      this.signOut();
    }
    if (this.authService.isLoggedIn()) {
      this.routerService.Profile.View();
    }
  }

  ngAfterViewInit(): void {
    this.profileForm.controls['login'].setErrors({ 'incorrect': false });
    this.profileForm.controls['password'].setErrors({ 'incorrect': false });
    setTimeout(() => this.inputLogin?.nativeElement.focus());
    this.initSubscribe();
  }

  public signIn(): void {
    if (!this.profileForm.valid) return;

    if (this.captchaEnabled && (this.captcha === "" || this.captcha.length === 0)) {
      this.snackService.open(`Докажите, что вы не робот!`);
      return;
    }

    this.appStateService.setIsLoadingState(true);
    const login = this.profileForm.controls['login'].value;
    const password = this.profileForm.controls['password'].value;

    this.authService.login(login, password)
      .pipe(
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => {
          this.routerService.Profile.View();
        },
        error: (errorContext) => {
          this.profileForm.setValue({login: this.profileForm.get('login')?.value, password:''});
          this.errorProcessor.Process(errorContext);
        }
      });
  }

  public signUp(): void {
    this.routerService.Profile.Register();
  }

  public getCaptchaResponse(captchaResponse: string): void {
    this.captcha = captchaResponse;
  }

  public signInByGoogle(): void {
    this.appStateService.setIsLoadingState(true);
    this.authService.signInByGoogle()
      .then((user: gapi.auth2.GoogleUser) => {
        const googleUser = this.initGoogleUser(user);
        if (googleUser.isLoggedIn) {
          this.authService.loginByGoogle(googleUser)
            .pipe(
              finalize(() => this.appStateService.setIsLoadingState(false)),
              takeUntil(this.destroy$)
            )
            .subscribe({
              next: () => this.routerService.Profile.View(),
              error: (err) => this.errorProcessor.Process(err, `Неизвестная ошибка при авторизация через Google сервис`),
            });
        }
      }).catch((error) => {
      if (error?.error?.detail) {
        const details: IProblemDetails = <IProblemDetails>error.error;
        this.snackService.open(details.detail);
      } else {
        this.snackService.open('Неизвестная ошибка при авторизация через Google сервис');
      }
    });
  }

  private signOut(): void {
    this.authService.logout();
    this.routerService.Profile.Login();
  }

  private initGoogleUser(user: gapi.auth2.GoogleUser): GoogleUser {
    const googleUser = new GoogleUser();
    if (user !== undefined) {

      const profile = user.getBasicProfile();
      googleUser.id = profile.getId();
      googleUser.fullName = profile.getName();
      googleUser.givenName = profile.getGivenName();
      googleUser.imageUrl = profile.getImageUrl();
      googleUser.email = profile.getEmail();

      const authResponse = user.getAuthResponse();
      googleUser.accessToken = authResponse.access_token;
      googleUser.expiresAt = authResponse.expires_at;
      googleUser.expiresIn = authResponse.expires_in;
      googleUser.firstIssuedAt = authResponse.first_issued_at;
      googleUser.TokenId = authResponse.id_token;
      googleUser.loginHint = authResponse.login_hint;
      googleUser.isLoggedIn = true;
    }

    return googleUser;
  }

  private initSubscribe(): void {
    this.authService.isGoogleClientEnabled()
      .pipe(takeUntil(this.destroy$))
      .subscribe((res: boolean) =>this.googleClientEnabled = res);
  }
}

