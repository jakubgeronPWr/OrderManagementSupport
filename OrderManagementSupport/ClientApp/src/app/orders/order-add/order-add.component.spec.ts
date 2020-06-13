import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderAddComponent } from './order-add.component';
import { FormBuilder } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import {HttpClientModule} from '@angular/common/http';

describe('OrderAddComponent', () => {
  let component: OrderAddComponent;
  let fixture: ComponentFixture<OrderAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [ OrderAddComponent ],
      providers: [
        FormBuilder
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('form invalid when empty', () => {
    expect(component.orderForm.valid).toBeFalsy();
  });

  it('service field validity', () =>{
    let errors = {};
    let service = component.orderForm.controls['service'];
    service.setValue("12345")
    errors = service.errors || {};
    expect(errors['minlength']).toBeTruthy();
  });

  it('price field validity', () => {
    let errors = {};
    let price = component.orderForm.controls['price'];
    errors = price.errors || {};
    expect(errors['required']).toBeTruthy();
  });
});
