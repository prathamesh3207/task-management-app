import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { TaskNav } from './Pages/task-nav/task-nav';
import { AuthServices } from './Services/auth-services';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,RouterLink,RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  private router = inject(Router);
  authService = inject(AuthServices);

  // Admin check (or move into AuthService as a computed signal)
  IsAdmin(): boolean {
    return localStorage.getItem('role') === 'admin';
  }

  // Current username for the navbar pill
  get currentUser(): string {
    return localStorage.getItem('username') || 'User';
  }

  logout(): void {
    localStorage.removeItem('IsLogin');
    localStorage.removeItem('role');
    localStorage.removeItem('username');
    localStorage.removeItem('token');

    this.authService.syncLoginState(); // Immediately hides the navbar
    this.router.navigate(['/Login']);
  }
}
