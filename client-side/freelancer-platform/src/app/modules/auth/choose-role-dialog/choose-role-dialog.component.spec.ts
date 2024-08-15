import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseRoleDialogComponent } from './choose-role-dialog.component';

describe('ChooseRoleDialogComponent', () => {
  let component: ChooseRoleDialogComponent;
  let fixture: ComponentFixture<ChooseRoleDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseRoleDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseRoleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
