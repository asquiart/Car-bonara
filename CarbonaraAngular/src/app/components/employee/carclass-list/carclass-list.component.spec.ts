import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarclassListComponent } from './carclass-list.component';

describe('CarclassListComponent', () => {
  let component: CarclassListComponent;
  let fixture: ComponentFixture<CarclassListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CarclassListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarclassListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
