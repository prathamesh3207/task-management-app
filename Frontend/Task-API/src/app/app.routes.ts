import { Routes } from '@angular/router';

export const routes: Routes = [

     { path: '', redirectTo: 'Login', pathMatch: 'full' },
  { 
    path: 'Login', 
    loadComponent: () => import('./Pages/login/login').then(m => m.Login) 
  },
  { 
    path: 'Add', 
    loadComponent: () => import('./Pages/task-add/task-add').then(m => m.TaskAdd) 
  },
  { 
  path: 'Edit/:id', 
  loadComponent: () => import('./Pages/task-edit/task-edit').then(m => m.TaskEdit) 
},

   { 
    path: 'List', 
    loadComponent: () => import('./Pages/task-list/task-list').then(m => m.TaskList) 
  },
   { 
    path: 'Register', 
    loadComponent: () => import('./Pages/register/register').then(m => m.Register) 
  },
  { path: '**', redirectTo: 'Login' }
];
