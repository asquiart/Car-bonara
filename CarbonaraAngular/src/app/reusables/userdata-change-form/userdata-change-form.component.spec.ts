import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserdataChangeFormComponent } from './userdata-change-form.component';

describe('UserdataChangeFormComponent', () => {
  let component: UserdataChangeFormComponent;
  let fixture: ComponentFixture<UserdataChangeFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserdataChangeFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserdataChangeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
