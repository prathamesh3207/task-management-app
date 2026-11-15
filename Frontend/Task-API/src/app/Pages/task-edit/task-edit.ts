import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TaskServices } from '../../Services/task-services';

@Component({
  selector: 'app-task-edit',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './task-edit.html',
  styleUrl: './task-edit.css',
})
export class TaskEdit implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private taskService = inject(TaskServices);

  taskId = signal<number>(0);
  message = signal('');

  // Individual signals bind cleanly with [(ngModel)] in Angular v21/v22
  title = signal('');
  description = signal('');
  isCompleted = signal('Pending');
  priority = signal('Medium');
  remark = signal('');
  createdDate = signal('');
  dueDate = signal('');
  username = signal('');

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    const numericId = Number(idParam);

    if (idParam && !isNaN(numericId)) {
      this.taskId.set(numericId);

      this.taskService.GetTaskEdit(idParam).subscribe({
        next: (res: any) => {
          const data = Array.isArray(res) ? res[0] : (res?.data ?? res);

          this.title.set(data.title ?? '');
          this.description.set(data.description ?? '');
          this.isCompleted.set(data.isCompleted ?? 'Pending');
          this.priority.set(data.priority ?? 'Medium');
          this.remark.set(data.remark ?? '');
          this.createdDate.set(data.createdDate ?? '');
          this.username.set(data.username ?? '');

          // Grab [0] so "2026-11-25T00:00:00" becomes "2026-11-25"
          this.dueDate.set(data.dueDate ? String(data.dueDate).split('T')[0] : '');
        },
        error: () => {
          this.message.set('Could not load task details.');
        }
      });
    }
  }

  onUpdateTask(): void {
    const updatedTask = {
      id: this.taskId(),
      title: this.title(),
      description: this.description(),
      isCompleted: this.isCompleted(),
      priority: this.priority(),
      remark: this.remark(),
      createdDate: this.createdDate(),
      dueDate: this.dueDate() || null,
      username: this.username()
    };

    this.taskService.UpdateTask(updatedTask as any).subscribe({
      next: () => {
        this.router.navigate(['/List']);
      },
      error: () => {
        this.message.set('Failed to update task.');
      }
    });
  }
}
