import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export interface ITaskList {
  id: number;
  title: string;
  description: string;
  isCompleted: string;
  priority: string;
  remark: string;
  createdDate: string;
  dueDate: string | null;
  Username: string | null;
}


@Injectable({
  providedIn: 'root',
})
export class TaskServices {

  private http = inject(HttpClient);
  private readonly apiURL = 'https://localhost:7241/api/Tasks';


GetAllTask(): Observable<ITaskList[]> {
  return this.http.get<ITaskList[]>(this.apiURL);
}

GetByIDTask(Username: string | null): Observable<ITaskList[]> {
  return this.http.get<ITaskList[]>(`${this.apiURL}/${Username}`);
}

DeleteTaskId(Id: number): Observable<any> {
  return this.http.delete(`${this.apiURL}/${Id}`);
}


addTask(task: ITaskList): Observable<any> {
  return this.http.post<any>(`${this.apiURL}/AddTask`, task);
}


UpdateTask(task: ITaskList): Observable<any> {
  return this.http.put<any>(`${this.apiURL}/EditTask`, task);
}

GetTaskEdit(Userid : string|null): Observable<ITaskList[]> {
  return this.http.get<ITaskList[]>(`${this.apiURL}/Getbyedit/${Userid}`);
}


}