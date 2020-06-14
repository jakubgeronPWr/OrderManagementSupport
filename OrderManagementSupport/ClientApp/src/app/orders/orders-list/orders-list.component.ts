import { Component, OnInit } from '@angular/core';
import { Order } from '../../shared/model/order';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit {

  orders: Order[];
  pageOfOrders: Array<any>;

  constructor(private readonly route: ActivatedRoute, private readonly http: HttpClient) { }

  ngOnInit(): void {
    this.orders = this.route.snapshot.data.orders;
    console.log(this.orders);
  }

  onChangePage(pageOfItems: Array<any>){
    this.pageOfOrders = pageOfItems;
  }

  deleteOrder(order){
    this.orders = this.orders.filter(x => x !== order);
    this.http.delete(`/api/orders/${order.orderId}`).subscribe(
      res =>{
          console.log(res);
      },
      err => {
        console.log(err.message);
    });
  }
}
