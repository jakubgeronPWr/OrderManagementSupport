# OrderManagementSupport
Application to help manage orders.

### Continous Integration Status: [![CircleCI](https://circleci.com/gh/jakubgeronPWr/OrderManagementSupport.svg?style=svg&circle-token=0dd5c3559b5c85ae71b11152caf187b534ad3411)](https://circleci.com/gh/jakubgeronPWr/OrderManagementSupport)

### Backend endpoints:
Method and Address | Behavior
------------ | -------------
GET ".../api/orders" | to get all orders
GET ".../api/orders/{id}" | to get order by id
POST ".../api/orders" | to add order, in body order with clientId 
DELETE ".../api/orders/{id}" | to delete order by id
