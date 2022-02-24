import '@angular/compiler';
import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from "../../services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {finalize} from "rxjs/operators";
import {ProblemDetails} from "../../models/ProblemDetails";
import {environment} from "../../../environments/environment";
import {SnackService} from "../../services/snack.service";
import {GoogleSigninService} from 'src/app/services/google-signin.service';
import {GoogleUserModel} from 'src/app/models/User/GoogleUserModel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})

export class LoginComponent implements OnInit  {

  @ViewChild('login', {static: true}) inputLogin:ElementRef | undefined;

  welcomeText: string = 'Добро пожаловать в систему Hackathon';

  profileForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl('')
  })

  isLoading: boolean = false;
  googleUser = new GoogleUserModel();
  isPassFieldHide: boolean = true;
  siteKey!: string;

  captchaEnabled:boolean = true
  captcha: string = "";

  constructor (private router: Router,
              private snackBar: MatSnackBar,
               private authService: AuthService,
              private googleSigninService: GoogleSigninService) {

    if (router.url === '/logout')
      this.SignOut();

    //if user is logged redirect to homepage
    if (this.authService.isLoggedIn())
      this.router.navigate(['/profile']);
  }

  ngOnInit(): void {
    this.googleSigninService.Observable().subscribe(user => {
      if(user != undefined) {
        var profile = user.getBasicProfile();
        this.googleUser.id = Number.parseInt(profile.getId());
        this.googleUser.fullName = profile.getName();
        this.googleUser.givenName = profile.getGivenName();
        this.googleUser.imageUrl = profile.getImageUrl();
        this.googleUser.email = profile.getEmail();
        var authResponse = user.getAuthResponse();
        this.googleUser.accessToken = authResponse.access_token;
        this.googleUser.expiresAt = authResponse.expires_at;
        this.googleUser.expiresIn = authResponse.expires_in;
        this.googleUser.firstIssuedAt = authResponse.first_issued_at;
        this.googleUser.TokenId = authResponse.id_token;
        this.googleUser.loginHint = authResponse.login_hint;
        this.googleUser.isLoggedIn = true;
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
          this.snackBar.open(errorMessage);
        });
  }

  signUp(){
    this.router.navigate(['/register']);
  }

  SignOut() {
    //this.googleSigninService.signOut();
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

}
