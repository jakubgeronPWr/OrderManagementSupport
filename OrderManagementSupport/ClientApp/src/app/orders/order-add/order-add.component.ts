import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
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
      'service': ['', Validators.required],
      'clientId': ['', Validators.required],
      'orderDate': [new Date(), Validators.required],
      'orderRealizationDate': ['', Validators.required],
      'price': ['', Validators.required],
      'isPayed': [false, Validators.required],
      'isDone': [false, Validators.required]
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
      res =>{
          console.log(res);
      },
      err => {
        console.log(err.message);
    });
  }

  ngOnInit(): void {

  }

}
