import { Injectable } from '@angular/core';
import { Customer } from '../modals/customer.modal';
import { HttpClient } from '@angular/common/http';
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  private localStorageData: string | null = "";
  
  private user: Customer;
  private login = false;
  private admin = false;
  private BASE_URL = "http://localhost:41223/api/Auth/";

  getUser(){
    return this.user;
  }
  
  isAdmin(){
    return this.admin;
  }

  isLOggedIn(){
    return this.login;
  }

  signUp(customer: Customer){
    return this.http.post<Customer>(this.BASE_URL+"signup",customer).pipe(map(res => {
      this.user = res;
      this.login = true;
      return this.login;
    }))
  }

  loginToSystem(customer: {email: string, password: string, isAdmin: number}){
    return this.http.post<Customer>(this.BASE_URL+"login",customer).pipe(map((res) => {
      this.user = res;
      this.login = true;
      this.admin = res.isAdmin === 1;
      return this.login;
    }));
  }
}
