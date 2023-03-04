import '@angular/compiler';
import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {ICreateUser} from 'src/app/models/User/CreateUser';
import {AuthService} from "../../services/auth.service";
import {Location} from "@angular/common";
import {IProblemDetails} from "../../models/IProblemDetails";
import {SnackService} from "../../services/snack.service";
import {takeUntil} from 'rxjs';
import {WithFormComponentBase} from "../WithFormComponentBase";
import {RouterService} from "../../services/router.service";
import {emailRegex} from "../../common/patterns/email-regex";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})

export class RegisterComponent extends WithFormComponentBase implements OnInit {

  @ViewChild('login', {static: true}) inputLogin: ElementRef | undefined;

  public override form: FormGroup = this.fb.group({});
  public welcomeText: string = 'Регистрация в системе Hackathon';
  public isLoading: boolean = false;
  public isPassFieldHide: boolean = true;

  private emailRegexp: RegExp = emailRegex;

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
      email: [null, [Validators.required, Validators.pattern(this.emailRegexp)]],
      fullname: [null, [Validators.required]],
    });

    this.form.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.isValid();
      })
  }
}
