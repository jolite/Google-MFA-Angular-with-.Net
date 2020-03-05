import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import {  Observable } from 'rxjs/observable';
import 'rxjs/add/operator/catch';

@Injectable({
  providedIn: 'root'
})
export class ApiserviceService {

  constructor(private router: Router, private http: HttpClient) { }

  loginService(userData: any) : Observable<any>{
    let url = 'https://localhost:44321/Home/LoginFromAngular';
    return this.http.post<any>(url,{login:userData})
    .catch(this.getError);    
  }

  MFAAuthenticatorService(passcode: any) : Observable<any> {
    let formData: FormData = new FormData();
    formData.append('passcode',passcode);
    let url = 'https://localhost:44321/Home/Verify2FAAngular?passCode='+passcode;
    return this.http.get<any>(url)
        .catch(this.getError);
  }

  private getError(error: Response): Observable<any>{
    console.log(error);
    return Observable.throw(error.json() || 'Server Issue');
  }
}
