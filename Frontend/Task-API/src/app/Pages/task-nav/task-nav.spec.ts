import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskNav } from './task-nav';

describe('TaskNav', () => {
  let component: TaskNav;
  let fixture: ComponentFixture<TaskNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskNav],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskNav);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
