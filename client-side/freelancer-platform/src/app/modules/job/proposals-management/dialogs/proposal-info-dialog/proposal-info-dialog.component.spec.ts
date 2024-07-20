import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProposalInfoDialogComponent } from './proposal-info-dialog.component';

describe('ProposalInfoDialogComponent', () => {
  let component: ProposalInfoDialogComponent;
  let fixture: ComponentFixture<ProposalInfoDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProposalInfoDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProposalInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
