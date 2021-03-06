
# Bussiness Case Design Document (initial draft)

## Introduction

After exploring a couple of ideas, we decided to make a own version of timesheet management app because this is the only software we all share knowledge about!  
We don't want to drive the concept too far but we still want to have some things be challenging enough to warant some thought about how to design it.

## Bussiness Domain

### Entities

####  User

|Property|Type|Required| 
|--|--|--|
|Id|int|yes
|FirstName|string(50)|yes
|LastName|string(50)|yes
|UserRoles|UserRole|  
 
#### UserRole

|Property|Type|Required| 
|--|--|--|
|Id|string(1)|yes 
|Description|string(100)|no

#### HolidayRequest

|Property|Type|Required| 
|--|--|--|
|Id|long|yes
|From|date|yes
|To|date|yes
|Remarks|string(200)|no
|IsApproved|bool|no
|DisApprovedReason|string(200)|no
|CreationDate|datetime|yes
|User|User|yes
|HolidayType|HolidayType|yes

#### HolidayType

|Property|Type|Required| 
|--|--|--|
|Id|string(1)|yes
|Description|string(30)|yes 

### Web

#### Functionalities

##### Login

- Employee
- Manager
- System administrator

##### Employee functionalities

- Register holiday
  - From date
  - Until date
  - HolidayType
  - Remarks
- List of holidays (by year)
  - Total number of approved holidays
  - Total number of disapproved holidays
  - Total number of holidays to be approved
  - Total number of approved holidays (grouped by HolidayType)
  - Overview of approved holidayrequests
    - From date
    - Until date
    - HolidayType
    - Remarks
  - Overview of holidayrequests to be approved
    - From date
    - Until date
    - HolidayType
    - Remarks
  - Overview of disapproved holidayrequests
    - From date
    - Until date
    - HolidayType
    - Remarks
	- DisapprovedReason
- Revoke holiday request

##### Manager functionalities

- Accept/Decline holiday request of employees
  - Approved/Declined
  - Decline reason
- Reports
  - List holidays by person (same as "List of holidays" of Employee)
  - List holidays (overview of all employees)

##### System administrator functionalities

- Change HolidayTypes (Paid holiday, Normal holiday, Sick-leave, Maternity leave)

#### Technical

##### Backend services

###### Register holiday 

- Validations
  - Default entity validations (required, max length,...)  
  - From date must be before To date
  - From date must be before today
  - Valid HolidayType
  - Holiday is not yet in database (to + from + user combination)

###### List of holidays (by year)

- Employee
  - Data
    - Total number of approved holidays	
    - Total number of disapproved holidays
    - Total number of holidays to be approved
    - Total number of approved holidays (grouped by HolidayType) (dynamic array)
    - Overview of approved holidayrequests
      - From date
      - Until date
      - HolidayType
      - Remarks
    - Overview of holidayrequests to be approved
      - From date
      - Until date
      - HolidayType
      - Remarks
	- Overview of disapproved holidayrequests
	  - From date
	  - Until date
	  - HolidayType
	  - Remarks
	  - DisapprovedReason
  - Filter
    - Year: if none provided (default): current year
	- User: authenticated user
  - Ordered by From date

##### Authentication (TODO)

To investigate:
- How to make the distinction between manager and employee and admin? Possible usertable in database with predefined users and assigned role
- Secure api calls
- Change api behaviour based on authentication

Information:
https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login

Because of previous experience and ease of use, we choose to use IdentityServer to host our security flows.  
https://github.com/IdentityServer

Our Identityserver setup will only work with hard-code testusers, but it can be hooked up to different identifycation protocols  ( Azure, Active Directory, Google, ...)

##### Authorization

Authorization is the process of assigning rights to indicate what a certain entity is allowed to do in your application. For instance is a person allowed to see certain data.

Here we want to make use of OAuth 2.0 access tokens. A nice features of Identity Server is that it is also a fully compliant  OAuth 2.0 provider.  This makes setting up an OpenID/ OAuth2.0 very easy to setup!



##### Screens

not needed for backend for the moment :-)


# EVERYTHING BELOW IS STILL TO VERIFY (OR ADD MORE DETAIL)

###### Revoke holiday request

- Validations
  - Default entity validations (required, max length,...) 
  - Not yet approved

###### Create User

- Validations
  - FirstName: length: 2-50
  - LastName: length: max 50
  - Combination of FirstName and LastName is unique.

###### List of holidays (by year)

- Manager: Same as for employee but extra filter: Employee

###### List of Holidays (overview of all employees)

Same as for employee but extra grouping level: Employee

###### Accept holiday request of employees

###### Decline holiday request of employees

- Validations
  - Decline reason

###### Change HolidayTypes

- Validations
  - Check uniqueness of HolidayType
- Initial values
  - Paid holiday
  - Normal holiday
  - Sick-leave
  - Maternity leave



##### TO DISCUSS

Do we need a more complex example (with a one-to-many relationship between "tables/entities")

Future functionalities or do we provide timesheets:
- Register timesheet (employee)
- List timesheets (employee)
- Accept/Decline timesheet of employees (manager)
- Change timesheet categories (application development, support,...)
- ...
