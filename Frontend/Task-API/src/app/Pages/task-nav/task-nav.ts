import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthServices } from '../../Services/auth-services';

@Component({
  selector: 'app-task-nav',
  imports: [RouterLink],
  templateUrl: './task-nav.html',
  styleUrl: './task-nav.css',
})
export class TaskNav {
 private router = inject(Router);

  authService = inject(AuthServices);

  UserID = signal<string|null>(localStorage.getItem('username'));

  roles = signal<string|null>(localStorage.getItem('role'));
   readonly IsAdmin = computed(() => this.roles() === 'Admin');

  

  logout(): void {
   
    
    localStorage.removeItem('username');
    localStorage.removeItem('IsLogin');
    localStorage.removeItem('role');
    localStorage.removeItem('role');
 this.authService.syncLoginState();
    this.router.navigate(['/Login']);
  }

}
