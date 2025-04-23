import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent {
  loginForm: FormGroup;
  error: string | undefined;

  constructor(private form: FormBuilder, private router: Router) {
    this.loginForm = this.form.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const username: string = this.loginForm.get('username')?.value;
      const password: string = this.loginForm.get('password')?.value;

      if (username.toLowerCase() === 'liam' && password.toLowerCase() === 'liam') {
        this.router.navigate(['/home']);
      }
      else {
        console.error('logging error');
        this.error = 'Try u: liam p: liam';
      }
    }
  }
}