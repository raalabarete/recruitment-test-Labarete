import React, { useState, useEffect} from 'react'

function EmployeeList(props) {
    const [value, setValue] = useState(props.employeeData); 
    function inputValidation(employeeName, employeeVal) {
        let isInputValid = true;
        if (!employeeName) {
            let nv = document.getElementById('nameValidation');
            nv.style.display = ''
            isInputValid = false;
        }
        if (parseInt(employeeVal) === 0) {
            let vzv = document.getElementById('valZeroValidation');
            vzv.style.display = ''
            isInputValid = false;
        }
        if (!Number.isInteger(parseInt(employeeVal))) {
            let vnv = document.getElementById('valNumValidation');
            vnv.style.display = ''
            isInputValid = false;
        }
        return isInputValid;
    }

    function clearValidation() {
        let nv = document.getElementById('nameValidation');
        nv.style.display = 'none'
        let vzv = document.getElementById('valZeroValidation');
        vzv.style.display = 'none'
        let vnv = document.getElementById('valNumValidation');
        vnv.style.display = 'none'
    }   
    function submitEmployee() {
        let employeeName = document.getElementById("empName").value;
        let employeeVal = document.getElementById("empVal").value;
        let hiddenName = document.getElementById("hiddenName").value;
        let hiddenVal = document.getElementById("hiddenNVal").value;
        let updateOrAdd = hiddenName === '' && hiddenVal === '' ?
            'addEmployee' :
            'updateEmployee'

        clearValidation();

        if (inputValidation(employeeName, employeeVal)) {

            fetch('http://localhost:41478/' + updateOrAdd, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name: employeeName,
                    value: parseInt(employeeVal),
                    prevName: hiddenName,
                    prevVal: parseInt(hiddenVal),
                }),
            })
                .then((response) => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response;
                })
                .then((data) => {
                    console.log('Success:', data);
                    return fetch('http://localhost:41478/Employees');
                }).then((response) => {
                    if (!response.ok) {
                        throw new Error('GET request failed');
                    }
                    return response.json();
                })
                .then((getData) => {
                    console.log('GET request successful:', getData);
                    setValue(getData);
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        }


    }



    function onClickEdit(name, value) {
        document.getElementById("hiddenName").value = name;
        document.getElementById("hiddenNVal").value = value;
        document.getElementById("empName").value = name;
        document.getElementById("empVal").value = value;
        clearValidation();
    }

    function onDelete(name, value) {
        fetch('http://localhost:41478/deleteEmployee', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                name: name,
                value: parseInt(value)
            }),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response;
            })
            .then((data) => {
                console.log('Success:', data);
                return fetch('http://localhost:41478/Employees');
            }).then((response) => {
                if (!response.ok) {
                    throw new Error('GET request failed');
                }
                return response.json();
            })
            .then((getData) => {
                console.log('GET request successful:', getData);
                setValue(getData);
            })
            .catch((error) => {
                console.error('Error:', error);
            });
    }

    function addEmployee() {
        document.getElementById("hiddenName").value = "";
        document.getElementById("hiddenNVal").value = "";
        document.getElementById("empName").value = "";
        document.getElementById("empVal").value = "0";
        clearValidation();
    }

  return (
      <div>
          <h1>Complete your app here</h1>
          <div>
              <input type="hidden" id="hiddenName"></input>
              <input type="hidden" id="hiddenNVal"></input>
          </div>
          <button onClick={addEmployee}>Add Employee</button>
          <div>
              <div>
                  <span>Name: </span>
                  <input id="empName"></input>
              </div>
              <span style={{ color: 'red', display: 'none' }} id="nameValidation">Enter a Name!</span>
          </div>
          <div>
              <div>
                  <span>Value: </span>
                  <input id="empVal"></input>
              </div>
              <div>
                  <span style={{ color: 'red', display: 'none' }} id="valZeroValidation">Value should not be 0!</span>
              </div>
              <div>
                  <span style={{ color: 'red', display: 'none' }} id="valNumValidation">Enter a Number!</span>
              </div>
          </div>
          <div>
              <button onClick={submitEmployee}>Submit</button>
          </div>
          <ul>
              {value.map((item, index) => (
                  <li key={index}>
                      {item.name}: {item.value} <button id="Edit" onClick={() => onClickEdit(item.name, item.value)}>Edit</button>
                      <button onClick={() => onDelete(item.name, item.value)}>Delete</button>
                  </li>

              ))}

          </ul>
      </div>
  )
}
export default EmployeeList
