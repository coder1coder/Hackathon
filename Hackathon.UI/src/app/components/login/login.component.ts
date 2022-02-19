import '@angular/compiler';
import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {MatSnackBar} from "@angular/material/snack-bar";
import {finalize} from "rxjs/operators";
import {ProblemDetails} from "../../models/ProblemDetails";
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
  isLoading: boolean = false;
  googleUser = new GoogleUserModel();

  constructor (private router: Router,
              private snackBar: MatSnackBar,
              private googleSigninService: GoogleSigninService) {

    if (router.url === '/logout')
      this.SignOut();

    if (this.googleUser.isLoggedIn)
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

      if (this.googleUser.isLoggedIn) {
        this.googleSigninService.login(this.googleUser)
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
              this.snackBar.open(errorMessage, "ok", { duration: 5 * 1000 });
            });
      }
    })
  }

  SignIn() {
    this.googleSigninService.signIn();
  }

  SignOut() {
    this.googleSigninService.signOut();
    this.router.navigate(['login']);
  }

  setLoading(isLoading:boolean){
    setTimeout(()=>{
      this.isLoading = isLoading;
    })
  }
}
