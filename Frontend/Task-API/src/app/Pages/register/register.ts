import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthServices, IRegister } from '../../Services/auth-services';

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('Password');
  const confirmPassword = control.get('ConfirmPassword');

  if (password && confirmPassword && password.value !== confirmPassword.value) {
    return { passwordMismatch: true };
  }
  return null;
};
@Component({
  selector: 'app-register',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
private SAuth = inject(AuthServices);

  Message = signal('');
  IsSuccess = signal(false);

  RegisterForm = new FormGroup(
    {
      Username: new FormControl('', { 
        nonNullable: true, 
        validators: [Validators.required] 
      }),
      Password: new FormControl('', { 
        nonNullable: true, 
        validators: [Validators.required, Validators.maxLength(10)] 
      }),
      ConfirmPassword: new FormControl('', { 
        nonNullable: true, 
        validators: [Validators.required, Validators.maxLength(10)] 
      }),
      Roles: new FormControl('', { 
        nonNullable: true, 
        validators: [Validators.required] 
      })
    },
    { validators: passwordMatchValidator }
  );

  get Username() { return this.RegisterForm.get('Username'); }
  get Password() { return this.RegisterForm.get('Password'); }
  get ConfirmPassword() { return this.RegisterForm.get('ConfirmPassword'); }
  get Roles() { return this.RegisterForm.get('Roles'); }

 OnRegister() {
  if (this.RegisterForm.invalid) {
    this.RegisterForm.markAllAsTouched();
    return;
  }

  // getRawValue() cleanly extracts all non-nullable form control values as IRegister
  const newUser: IRegister = this.RegisterForm.getRawValue();

  this.SAuth.AddUser(newUser).subscribe({
    next: (res) => {
      this.IsSuccess.set(true);
      const serverMsg = res.body?.message;

      switch (res.status) {
        case 200:
        case 201:
          this.Message.set(serverMsg || 'Registration successful! You can now log in.');
          this.RegisterForm.reset();
          break;
        default:
          this.Message.set(serverMsg || 'User registered.');
      }
    },
    error: (err) => {
      this.IsSuccess.set(false);
      // Extract .message whether err.error is a JSON object or plain string
      const apiError = err.error?.message || (typeof err.error === 'string' ? err.error : null);

      switch (err.status) {
        case 400:
          this.Message.set(apiError || 'Invalid registration details.');
          break;
        case 401:
          this.Message.set(apiError || 'Unauthorized request.');
          break;
        case 409:
          this.Message.set(apiError || 'Username already exists. Please choose another.');
          break;
        case 500:
          this.Message.set('Server error. Please try again later.');
          break;
        case 0:
          this.Message.set('Unable to connect to the server. Check your network.');
          break;
        default:
          this.Message.set(apiError || 'Register API Failed.');
      }

      console.error('Register API Failed:', err);
    }
  });
}

  OnReset() {
  this.RegisterForm.reset();
  this.Message.set('');
}


}
