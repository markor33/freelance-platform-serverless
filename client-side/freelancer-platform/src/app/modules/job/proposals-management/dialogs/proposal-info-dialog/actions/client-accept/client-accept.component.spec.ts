import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAcceptComponent } from './client-accept.component';

describe('ClientAcceptComponent', () => {
  let component: ClientAcceptComponent;
  let fixture: ComponentFixture<ClientAcceptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientAcceptComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientAcceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
