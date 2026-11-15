import { Component, computed, inject, signal } from '@angular/core';
import { ITaskList, TaskServices } from '../../Services/task-services';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-list',
  imports: [RouterLink,CommonModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList {
  private tasklistservice = inject(TaskServices);
  UserID = signal<string|null>(localStorage.getItem('username'));

  roles = signal<string|null>(localStorage.getItem('role'));
   readonly IsAdmin = computed(() => this.roles() === 'Admin');


 Message = signal('');
 SearchBy = signal('');

  Lists = signal<ITaskList[]>([]);

ngOnInit(): void {
    console.log(this.IsAdmin());

    if (this.IsAdmin()) {
      this.LoadList();
    } else {
      this.LoadListById(this.UserID());
    }
  }
  LoadListById(Username:string | null){
    this.tasklistservice.GetByIDTask(Username).subscribe({
      next : (res : ITaskList[]) => {
        this.Lists.set(res);
      },
       error : (err) =>{
        this.Message.set(err);
        console.error(err);
      }
    })
  }

    LoadList(){
    this.tasklistservice.GetAllTask().subscribe({
      next : (res : ITaskList[]) => {
        this.Lists.set(res);
      },
      error : (err) =>{
        this.Message.set(err);
        console.error(err);
      }
    })
  }

 OnDelete(ID: number): void {
  this.tasklistservice.DeleteTaskId(ID).subscribe({
    next: (res: any) => {
      // 1. Extract the message string from TaskResponseDTO
      this.Message.set(res?.message || 'Task deleted successfully.');

      // 2. Remove the deleted task from the table immediately
      this.Lists.update(tasks => tasks.filter(t => t.id !== ID));
    },
    error: (err) => {
      // Extract the message string from BadRequest / NotFound / 500 TaskResponseDTO
      const errorMsg = err?.error?.message || 'Failed to delete task.';
      this.Message.set(errorMsg);
      console.error(err);
    }
  });
}


  OnSerch(Username:string){
    this.tasklistservice.GetTaskEdit(Username).subscribe({
      next : (res : ITaskList[]) => {
        this.Lists.set(res);
      },
       error : (err) =>{
        this.Message.set(err);
        console.error(err);
      }
    })
  }

  Reset(){
    this.Message.set('');
    this.LoadList();
  }

}
