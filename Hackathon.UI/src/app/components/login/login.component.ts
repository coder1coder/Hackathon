import '@angular/compiler';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
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

  isLoading: boolean = false;
  isPassFieldHide: boolean = true;

  constructor(private router: Router, private authService: AuthService,
              private snackBar: MatSnackBar) {

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

  welcomeText: string = 'Добро пожаловать в систему Hackathon';

  profileForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl(''),
  })

  signIn(){
    if (!this.profileForm.valid){
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
          let details: ProblemDetails = <ProblemDetails>error.error;

          this.profileForm.setValue({login: this.profileForm.get('login')?.value, password:''});

          this.snackBar.open(details.detail, "ok", { duration: 5 * 1000 });
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

}
