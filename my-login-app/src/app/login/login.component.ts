import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import { LoginModel} from './login.model';
import {ApiserviceService} from '../apiservice.service';
import { FormsModule, FormControl, FormGroup } from '@angular/forms';
import { NgxQRCodeModule } from 'ngx-qrcode2';
// import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  elementType : 'url' | 'canvas' | 'img' = 'url';
  value : string = '';
  username: string;
  password: string;
  UserDetails=new LoginModel();
  errorMessage: string;
  MfaData:any;
  Mfastatus:boolean = false;
  MfaCode: any;

  MFAForm: any = new FormGroup({
    passcode: new FormControl('')
  });
  constructor(private router: Router,private apiserv:ApiserviceService) { }
  
  ngOnInit() {
  }
  login() {
    
    this.UserDetails.UserName=this.username;
    this.UserDetails.Password=this.password;    
    this.apiserv.loginService(this.UserDetails).subscribe((data) => {
      if(data.status) { 
          this.MfaData = data;      
          this.Mfastatus = data.status;
          this.value = this.MfaData.BarcodeImageUrl;
          localStorage.setItem('UserSession',this.UserDetails.UserName);
          
        }    
        else{    
          this.errorMessage = data.Message;    
        }    
      },    
      error => {    
        this.errorMessage = error.message;
    });
  }
  MFAAuthenticator(){
    
    this.apiserv.MFAAuthenticatorService(this.MFAForm.get('passcode').value).subscribe((data) => {
      if(data) {       
        this.router.navigate(['/user']);
      }    
      else {
        alert('Please Enter Correct 6 digit code!!')    
      }    
      },    
      error => {    
        this.errorMessage = error.message;
    })
  }

}
