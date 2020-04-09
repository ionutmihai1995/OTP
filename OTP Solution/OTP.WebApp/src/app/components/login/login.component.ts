import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FormHelperService } from 'src/app/services/form-helper.service';
import { UserService } from 'src/app/services/user.service';
import { ValidityResult } from 'src/app/models/validity-result';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  checkUserValidityForm: FormGroup;
  OTPForm: FormGroup;
  loading: boolean = false;
  requestError: string;
  validityResult: ValidityResult;
  timerSeconds;
  isLoggedIn: boolean = false;
  successfullyLoginMsg: string;
  constructor(
    private formBuilder: FormBuilder,
    private formHelperService: FormHelperService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.checkUserValidityForm = this.formBuilder.group({
      userId: ['', [Validators.required]],
    });

    this.OTPForm = this.formBuilder.group({
      otp: ['', [Validators.required]],
    });
  }

  showErrorMessage(form: FormGroup, controlName: string, validator: string): boolean {
    return this.formHelperService.showErrorMessage(form, controlName, validator)
  }

  checkValidity() {
    //show loading indicator
    this.loading = true;
    this.requestError = '';
    setTimeout(() => {
      this.userService.checkValidity(this.checkUserValidityForm.value.userId).subscribe(result => {
        this.validityResult = result;
        //get difference remaining
        this.timerSeconds = (new Date(result.expiredDate).getTime() - new Date().getTime()) / 1000;
        this.loading = false;
      }, (error) => {
        if (error.status == 400)
          this.requestError = error.error;
        this.loading = false;
      });
    }, 1000);
  }

  submit() {
    this.loading = true;
    this.userService.login(this.checkUserValidityForm.value.userId, this.OTPForm.value.otp, this.validityResult.secretKeyGuid).subscribe(_ => {
      this.loading = false;
      this.isLoggedIn = true;
      this.successfullyLoginMsg = 'Successfully login';
    }, (error) => {
      if (error.status == 400)
        this.requestError = error.error;
      this.loading = false;
    });
  }

}
