import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { MfaService } from 'src/app/services/mfa.service';

declare var $:any;

@Component({
  selector: 'app-mfa',
  templateUrl: './mfa.component.html',
  styleUrls: ['./mfa.component.css']
})
export class MfaComponent implements OnInit {

  mfaData: any;
  mfaMessage: any;
  otp: string = '';
  userId: any;
  mfaPost: any;

  constructor(
    private mfaService: MfaService
  ) { 
    this.userId = sessionStorage.getItem('USERID');
  }

  ngOnInit(): void {
  }
  mfaClose(event: any) {
    event.preventDefault();
    sessionStorage.removeItem('USERID')
    sessionStorage.removeItem('USERNAME')
    sessionStorage.removeItem('TOKEN')
    sessionStorage.removeItem('USERPIC')
    this.mfaMessage = '';
    $("#mfareset").click();
  }

  submitMfaForm(mfaForm:NgForm) {
    if (mfaForm.value.otp === "") {
      this.mfaMessage = "Enter 6 digits OTP Code.";
      window.setTimeout(() => {
        this.mfaMessage = '';
      }, 3000);
      return;
    }
    if (mfaForm.valid) {
      this.mfaData = mfaForm.value;
      if (this.mfaData.otp === '') {
        this.mfaMessage="Please enter OTP Code."
        window.setTimeout(() => {
          this.mfaMessage=null;
        }, 3000);
        return;
      }

      this.mfaPost = {
        id: this.userId,
        otp: this.mfaData.otp
      }
      this.mfaService.sendMfaValidationRequest(this.mfaPost).subscribe((res: any) => {
        if (res.statuscode == 200) {
          this.mfaMessage = res.message;
          sessionStorage.setItem("USERNAME", res.username);
          window.setTimeout(() => {
            window.location.reload();
          }, 3000);

        } else {
          this.mfaMessage = res.message;
          window.setTimeout(() => {
            this.mfaMessage = null;
          }, 3000);
        }
      }
      )
    }
  }
}
