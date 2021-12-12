import '@angular/compiler';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { CreateUser } from 'src/app/models/CreateUser';
import {AuthService} from "../../services/auth.service";
import {Location} from "@angular/common";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ProblemDetails} from "../../models/ProblemDetails";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})

export class RegisterComponent implements AfterViewInit  {

  isLoading: boolean = false;
  isPassFieldHide: boolean = true;

  welcomeText: string = 'Регистрация в системе Hackathon';

  registerForm = new FormGroup({
    login: new FormControl(''),
    password: new FormControl(''),
    email: new FormControl(''),
    fullname: new FormControl(''),
  })

  @ViewChild('login', {static: true}) inputLogin:ElementRef | undefined;

  constructor(private router: Router,
              private location: Location,
              private authService: AuthService,
              private snackbar: MatSnackBar) {
    //if user is logged redirect to homepage
    if (this.authService.isLoggedIn())
      router.navigate(['/profile']);
  }
  ngAfterViewInit(): void {

    this.registerForm.controls['login'].setErrors({ 'incorrect': false });
    this.registerForm.controls['password'].setErrors({ 'incorrect': false });
    this.registerForm.controls['email'].setErrors({ 'incorrect': false });
    this.registerForm.controls['fullname'].setErrors({ 'incorrect': false });

    setTimeout((_: any) => {
      this.inputLogin?.nativeElement.focus();
    })
  }

  signUp(){
    let createUser = new CreateUser();
    createUser.userName = this.registerForm.get("login")?.value;
    createUser.password = this.registerForm.get("password")?.value;
    createUser.email = this.registerForm.get("email")?.value;
    createUser.fullname = this.registerForm.get("fullname")?.value;

    this.authService.register(createUser)
      .subscribe(r=>{
        this.snackbar.open(`Пользователь успешно зарегистрирован с ID: ${r.id}`);
        setTimeout(()=>{
          this.router.navigate(['/login']);
        },1000);
      },
        error=>{

          let problemDetails: ProblemDetails = <ProblemDetails>error.error;
          this.snackbar.open(problemDetails.detail,"OK");

        });
  }

  goBack(){
    this.location.back();
  }

  setLoading(isLoading:boolean){
    setTimeout(()=>{
      this.isLoading = isLoading;
    })
  }

}
