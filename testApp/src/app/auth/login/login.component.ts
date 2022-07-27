import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm = new FormGroup({
    email: new FormControl('',[Validators.required]),
    password: new FormControl('',[Validators.required]),
    role: new FormControl('',[Validators.required])
  });

  constructor(private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {}

  reset(){
    this.loginForm.reset();
  }

  login(){
    let user = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
      isAdmin: +this.loginForm.value.role
    }
    
    let found = false;
    this.authService.loginToSystem(user).subscribe(logedIn => {
      found = logedIn;
      if(found){
        alert("Login successfull...");
        if(user.isAdmin === 0){
          this.router.navigate(['/cust']);
        }else{
          this.router.navigate(['/admin'])
        }
      }else{
        alert("Invalid Email or password...");
        this.reset();
        return;
      }
    });
  }
}
