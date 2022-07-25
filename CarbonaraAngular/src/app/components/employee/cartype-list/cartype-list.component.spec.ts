import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CartypeListComponent } from './cartype-list.component';

describe('CartypeListComponent', () => {
  let component: CartypeListComponent;
  let fixture: ComponentFixture<CartypeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CartypeListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CartypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
