import { Component, inject, signal, Signal } from '@angular/core';
import { AuthServices, ILogin, ILOGININ } from '../../Services/auth-services';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { maxLength } from '@angular/forms/signals';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private SAuth = inject(AuthServices);

  private router = inject(Router);
Message = signal('');
IsSuccess = signal(false);


  LoginForm = new FormGroup({
    Username : new FormControl('',{ nonNullable: true, validators: [Validators.required] }),
     Password: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(10)] })
  })

  get Username(){
    return this.LoginForm.get('Username');
  }
   get Password(){
    return this.LoginForm.get('Password');
  }


 OnLogin() {
    if (this.LoginForm.invalid) {
      this.LoginForm.markAllAsTouched();
      return;
    }

    const loginValid: ILogin = this.LoginForm.getRawValue();

    this.SAuth.CheckLogin(loginValid).subscribe({
      next: (res: ILOGININ) => {
        this.IsSuccess.set(true);
        this.Message.set(res?.message || 'Login successful!');
        localStorage.setItem('username',res?.username);
        localStorage.setItem('role',res?.roles);
        localStorage.setItem('IsLogin', 'true');
        this.SAuth.syncLoginState(); 
   
      

        // Navigate to your home/dashboard route after login
         this.router.navigate(['/List']);
      },
      error: (err) => {
        this.IsSuccess.set(false);
        const apiError = err.error?.message || (typeof err.error === 'string' ? err.error : null);

        switch (err.status) {
          case 400:
            this.Message.set(apiError || 'Invalid login request.');
            break;
          case 401:
            this.Message.set(apiError || 'Invalid username or password.');
            break;
          case 404:
            this.Message.set(apiError || 'User not found.');
            break;
          case 500:
            this.Message.set('Internal server error. Please try again later.');
            break;
          case 0:
            this.Message.set('Server unreachable. Check your connection.');
            break;
          default:
            this.Message.set(apiError || 'Login API Failed.');
        }

        console.error('Login API Failed:', err);
      }
    });
  }
   OnReset() {
  this.LoginForm.reset();
  this.Message.set('');
}


}
