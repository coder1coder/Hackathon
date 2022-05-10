import '@angular/compiler';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ICreateUser } from 'src/app/models/User/CreateUser';
import { AuthService } from "../../services/auth.service";
import { Location } from "@angular/common";
import { IProblemDetails } from "../../models/IProblemDetails";
import { SnackService } from "../../services/snack.service";
import { takeUntil } from 'rxjs';
import {ComponentBase} from "../ComponentBase";
import {RouterService} from "../../services/router.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})

export class RegisterComponent extends ComponentBase implements OnInit {
  public override form: FormGroup = this.fb.group({});
  public welcomeText: string = 'Регистрация в системе Hackathon';
  public isLoading: boolean = false;
  public isPassFieldHide: boolean = true;

  @ViewChild('login', {static: true}) inputLogin: ElementRef | undefined;

  constructor(
    private routerService: RouterService,
    private location: Location,
    private authService: AuthService,
    private snackbar: SnackService,
    private fb: FormBuilder) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
  }

  public signUp(): void {
    const createUser: ICreateUser = {
      userName: this.getFormControl('login').value,
      password: this.getFormControl('password').value,
      email: this.getFormControl('email').value,
      fullname: this.getFormControl('fullname').value
    };

    this.authService.register(createUser)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) =>{
          this.snackbar.open(`Пользователь успешно зарегистрирован с ID: ${res.id}`);
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackbar.open(problemDetails.detail);
        },
        complete: () => {
          setTimeout(()=>{
            this.routerService.Profile.Login();
          }, 1000);
        }
      });
  }

  public goBack(): void {
    this.location.back();
  }

  public isValid(): boolean {
    return this.form.valid;
  }

  private initForm(): void {
    this.form = this.fb.group({
      login: [null, [Validators.required]],
      password: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]],
      fullname: [null, [Validators.required]],
    });

    this.form.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.isValid();
      })
  }
}
