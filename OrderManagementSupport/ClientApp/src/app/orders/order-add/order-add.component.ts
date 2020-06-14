import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Order } from '../../shared/model/order';

@Component({
  selector: 'app-order-add',
  templateUrl: './order-add.component.html',
  styleUrls: ['./order-add.component.css']
})
export class OrderAddComponent implements OnInit {

  orderForm: FormGroup;

  service: AbstractControl;
  clientId: AbstractControl;
  orderDate: AbstractControl;
  orderRealizationDate: AbstractControl;
  price: AbstractControl;
  isPayed: AbstractControl;
  isDone: AbstractControl;


  constructor(formBuilder: FormBuilder, private readonly http: HttpClient) {
    this.orderForm = formBuilder.group({
      'service': ['', [Validators.required, Validators.minLength(6)]],
      'clientId': ['', Validators.required],
      'orderDate': [new Date(), Validators.required],
      'orderRealizationDate': [new Date(), Validators.required],
      'price': ['', Validators.required],
      'isPayed': [false],
      'isDone': [false]
    });

    this.service = this.orderForm.controls['service'];
    this.clientId = this.orderForm.controls['clientId'];
    this.orderDate = this.orderForm.controls['orderDate'];
    this.orderRealizationDate = this.orderForm.controls['orderRealizationDate'];
    this.price = this.orderForm.controls['price'];
    this.isPayed = this.orderForm.controls['isPayed'];
    this.isDone = this.orderForm.controls['isDone'];
  }

  onSubmit(order: Order) {
    this.http.post<Order>('/api/orders', order).subscribe(
      res => {
        console.log(res);
        alert("Order has been added");
      },
      err => {
        console.log(err.message);
        alert("Order date can not be earlier than 7 days and later than 7 days from today. Realization date can not be earlier than order date and later than 90 days after order date");
      });
  }

  ngOnInit(): void {

  }

}
