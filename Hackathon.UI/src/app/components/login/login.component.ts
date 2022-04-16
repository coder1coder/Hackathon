import '@angular/compiler';
import {AfterViewInit, Component, ElementRef, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from "../../services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {finalize} from "rxjs/operators";
import {ProblemDetails} from "../../models/ProblemDetails";
import {environment} from "../../../environments/environment";
import {GoogleUserModel} from 'src/app/models/User/GoogleUserModel';
import {FormControl, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})

export class LoginComponent implements AfterViewInit  {

  @ViewChild('login', {static: true}) inputLogin:ElementRef | undefined;
  welcomeText: string = 'Добро пожаловать в систему Hackathon';

  profileForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl('')
  })

  isLoading: boolean = false;
  isPassFieldHide: boolean = true;
  siteKey!: string;

  captchaEnabled:boolean = environment.captchaEnabled;
  captcha: string = "";

  constructor (private router: Router,
              private snackBar: MatSnackBar,
              private authService: AuthService,
              ) {

    if (router.url === '/logout')
      this.signOut();

    //if user is logged redirect to homepage
    if (this.authService.isLoggedIn())
      this.router.navigate(['/profile']);
  }

  ngAfterViewInit(): void {
    this.profileForm.controls['login'].setErrors({ 'incorrect': false });
    this.profileForm.controls['password'].setErrors({ 'incorrect': false });
    setTimeout(() => this.inputLogin?.nativeElement.focus())
  }

  signIn(){
    if (!this.profileForm.valid) return;

    if (this.captchaEnabled && (this.captcha === "" || this.captcha.length === 0)) {
      this.snackBar.open("Докажите, что вы не робот!");
      return;
    }

    this.setLoading(true);

    let login = this.profileForm.controls['login'].value;
    let password = this.profileForm.controls['password'].value;

    this.authService.login(login, password)
      .pipe(
        finalize(() => this.setLoading(false))
      )
      .subscribe(_ => {
          this.router.navigate(['/profile']);
        },
        error => {
          let errorMessage = "Неизвестная ошибка";

          if (error.error.detail !== undefined) {
            let details: ProblemDetails = <ProblemDetails>error.error;
            errorMessage = details.detail;
          }

          this.profileForm.setValue({login: this.profileForm.get('login')?.value, password:''});
          this.snackBar.open(errorMessage, "ok", { duration: 5 * 500 });
        });
  }

  signUp(){
    this.router.navigate(['/register']);
  }

  signOut() {
    this.authService.logout();
    this.router.navigate(['login']);
  }

  setLoading(isLoading:boolean){
    setTimeout(()=>{
      this.isLoading = isLoading;
    })
  }

  getCaptchaResponse(captchaResponse: string) {
    this.captcha = captchaResponse;
  }

  signInByGoogle() {
    this.authService.signInByGoogle()
      .then(user => {
        let googleUser = this.initGoogleUser(user);
        if(googleUser.isLoggedIn) {
          this.authService.loginByGoogle(googleUser)
          .subscribe(_ => {
              this.router.navigate(['/profile']);
            },
            error => {
              let errorMessage = "Неизвестная ошибка при авторизация через Google сервис";
              if (error.error.detail !== undefined) {
                let details: ProblemDetails = <ProblemDetails>error.error;
                errorMessage = details.detail;
              }
              this.snackBar.open(errorMessage, "ok", { duration: 5 * 500 });
            });
        }
      })
  }

  initGoogleUser(user: any): GoogleUserModel {
    let googleUser = new GoogleUserModel();
    if(user != undefined) {
      let profile = user.getBasicProfile();
      googleUser.id = profile.getId();
      googleUser.fullName = profile.getName();
      googleUser.givenName = profile.getGivenName();
      googleUser.imageUrl = profile.getImageUrl();
      googleUser.email = profile.getEmail();
      let authResponse = user.getAuthResponse();
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
}
