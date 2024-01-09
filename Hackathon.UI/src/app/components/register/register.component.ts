import '@angular/compiler';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ICreateUser } from 'src/app/models/User/CreateUser';
import { AuthService } from "../../services/auth.service";
import { Location } from "@angular/common";
import { SnackService } from "../../services/snack.service";
import { Observable, takeUntil } from 'rxjs';
import { WithFormBaseComponent } from "../../common/base-components/with-form-base.component";
import { RouterService } from "../../services/router.service";
import { emailRegex } from "../../common/patterns/email-regex";
import { fromMobx } from "../../common/functions/from-mobx.function";
import { AppStateService } from "../../services/app-state.service";
import { ErrorProcessorService } from "../../services/error-processor.service";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})

export class RegisterComponent extends WithFormBaseComponent implements OnInit {

  @ViewChild('login', {static: true}) inputLogin: ElementRef;

  public override form: FormGroup = this.fb.group({});
  public welcomeText: string = 'Регистрация в системе Hackathon';
  public isPassFieldHide: boolean = true;
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);

  private emailRegexp: RegExp = emailRegex;

  constructor(
    private routerService: RouterService,
    private location: Location,
    private authService: AuthService,
    private snackbar: SnackService,
    private fb: FormBuilder,
    private appStateService: AppStateService,
    private errorProcessor: ErrorProcessorService,
  ) {
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
      fullName: this.getFormControl('fullName').value
    };
    this.appStateService.setIsLoadingState(true);
    this.authService.register(createUser)
      .pipe(
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res) => {
          this.snackbar.open(`Пользователь успешно зарегистрирован с ID: ${res.id}`);
          setTimeout(() => {
            this.routerService.Profile.Login();
          }, 1000);
        },
        error: (error) => this.errorProcessor.Process(error),
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
      fullName: [null, [Validators.required]],
    });

    this.form.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.isValid());
  }
}
