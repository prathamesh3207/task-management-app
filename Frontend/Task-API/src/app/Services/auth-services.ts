import { HttpClient, HttpResponse } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';

export interface ILogin {
  Username: string;
  Password: string;
}

export interface IRegister {
  Username: string;
  Password: string;
  ConfirmPassword: string;
  Roles: string;
}


export interface IAuthResponse {
  message: string;

}
export interface ILOGININ {
  isSuccess: boolean;
  message: string;
  username: string;
  roles: string;
}



@Injectable({
  providedIn: 'root',
})
export class AuthServices {

   private login = signal<string | null>(localStorage.getItem('IsLogin'));
  readonly IsLogin = computed(() => this.login() === 'true');

  syncLoginState(): void {
    this.login.set(localStorage.getItem('IsLogin'));
  }
  private http = inject(HttpClient);
  private readonly apiURL = 'https://localhost:7241/api/Auth/';

 CheckLogin(login: ILogin): Observable<ILOGININ> {
  return this.http.post<ILOGININ>(`${this.apiURL}Login`, login);
}

  AddUser(register: IRegister): Observable<HttpResponse<IAuthResponse>> {
    return this.http.post<IAuthResponse>(this.apiURL + 'Register', register, {
      observe: 'response'
    });
  }
}
