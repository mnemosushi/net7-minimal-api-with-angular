import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { Input, Ripple, initTE } from 'tw-elements';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginFormGroup = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  submit() {
    console.log('is form valid?', this.loginFormGroup.valid);
    if (this.loginFormGroup.valid) {
      console.log('submit button pressed: ', this.loginFormGroup.value);
    }
  }

  ngOnInit() {
    initTE({ Input, Ripple });
  }
}
