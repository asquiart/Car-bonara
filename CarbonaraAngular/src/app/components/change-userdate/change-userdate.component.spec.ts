import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeUserdateComponent } from './change-userdate.component';

describe('ChangeUserdateComponent', () => {
  let component: ChangeUserdateComponent;
  let fixture: ComponentFixture<ChangeUserdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeUserdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeUserdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
