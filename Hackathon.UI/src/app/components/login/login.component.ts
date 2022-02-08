import '@angular/compiler';
import {AfterViewInit, Component, ElementRef, ViewChild} from '@angular/core';
import {FormGroup, FormControl, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthService} from "../../services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {finalize} from "rxjs/operators";
import {ProblemDetails} from "../../models/ProblemDetails";

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
  captcha: string = "";

  constructor(private router: Router, private authService: AuthService,
              private snackBar: MatSnackBar) {

    this.siteKey = '6Lew_2YeAAAAAFuRFhML7FxNeiD4wZnrX6x9GF_a';

    if (router.url === '/logout')
      this.#logout();

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
    if (!this.profileForm.valid || this.captcha === "" || this.captcha.length === 0) {
      this.snackBar.open("Докажите, что вы не робот!", "ok", { duration: 5 * 500 });
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

          if (error.error.details !== undefined) {
            let details: ProblemDetails = <ProblemDetails>error.error;
            errorMessage = details.detail;
          }

          this.profileForm.setValue({login: this.profileForm.get('login')?.value, password:''});
          this.snackBar.open(errorMessage, "ok", { duration: 5 * 1000 });
        });
  }

  signUp(){
    this.router.navigate(['/register']);
  }

  #logout(){
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
