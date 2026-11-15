import { Component, inject, signal } from '@angular/core';
import { TaskServices } from '../../Services/task-services';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-add',
  imports: [FormsModule,RouterLink],
  templateUrl: './task-add.html',
  styleUrl: './task-add.css',
})
export class TaskAdd {
 private taskService = inject(TaskServices);
  private router = inject(Router);

  message = signal('');

  UserID = signal<string|null>(localStorage.getItem('username'));
  task = {
    Title: '',
    Description: '',
    IsCompleted: 'Pending',
    Priority: 'Medium',
    Remark: '',
    CreatedDate: new Date().toISOString().split('T')[0],
    DueDate: '',
    Username:this.UserID()
  };

  onAddTask(): void {
    this.taskService.addTask(this.task as any).subscribe({
      next: () => {
        this.router.navigate(['/List']);
      },
      error: () => {
        this.message.set('Failed to create task. Please try again.');
      }
    });
  }

}
