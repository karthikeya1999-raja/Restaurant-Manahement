import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  signupForm = new FormGroup({
    name: new FormControl('',[Validators.required]),
    phone: new FormControl('',[
      Validators.required, 
      Validators.pattern('[0-9]{10}')]),
    email: new FormControl('',[
      Validators.required,
      Validators.email]),
    password: new FormControl('',[
      Validators.required,
      Validators.minLength(6)]),
    cpassword: new FormControl(''),
  })

  constructor(private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
  }

  signUp(){
    let customer = {
      name: this.signupForm.value.name,
      phone: this.signupForm.value.phone,
      email: this.signupForm.value.email,
      password: this.signupForm.value.password,
      isAdmin: 0
    }
    this.authService.signUp(customer).subscribe(signup => {
      if(signup){
        alert("Signup Successfull....");
        this.reset();
        this.router.navigate(['/cust']);
      }
    });
  }

  reset(){
    this.signupForm.reset();
  }
}
