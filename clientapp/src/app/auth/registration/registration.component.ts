import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';

declare var $:any;

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registrationData: any;
  registrationMessage: any; 

  constructor(
    private registrationService: RegistrationService,
  ) { }

  ngOnInit(): void {
  }

  registrationClose(event: any) {
    event.preventDefault();
    $("#regiter-reset").click();
  }

  submitRegistrationForm(registrationForm:NgForm) {
    if (registrationForm.value.firstname === "" || 
        registrationForm.value.lasttname === "" ||
        registrationForm.value.email === "" ||
        registrationForm.value.mobile === "" ||
        registrationForm.value.username === "" ||
        registrationForm.value.password === "") {
          this.registrationMessage = "Please fill up and complete registration form.";
          window.setTimeout(() => {
            this.registrationMessage = "";
          }, 3000);
          return;
        }

      const xmail = registrationForm.value.email.toString();
     if (!xmail.includes('@') || !xmail.includes('.')) {
        this.registrationMessage = "Invalid Email Address format.";
        window.setTimeout(() => {
          this.registrationMessage = "";
        }, 3000);
        return;
      }

    if(registrationForm.valid)
    {
       this.registrationData = registrationForm.value;

       this.registrationService.sendRegistrationRequest(this.registrationData).subscribe(res => {
        if(res.statuscode == 200) {
          this.registrationMessage = res.message;
          return;
        } else {
          this.registrationMessage = res.message;
          setTimeout(() => {
            this.registrationMessage = null;            
          }, 3000);

        }

      });     
    }
  }
}
