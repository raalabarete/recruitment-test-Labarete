import React, { useState, useEffect } from 'react'

export default function ABCEmployeeList() {
    const [value, setValue] = useState([]);


    useEffect(() => {
        fetch("http://localhost:41478/List", {
            method: 'GET',
            headers: { 'Content-type': 'application/json' },
        })
            .then((resp) => resp.json())
            .then((json) => {
                setValue(json);
            })
    }, []);

  return (
      <div>
      <h2> List of Employees that starts with A, B, and C</h2>
          <ul>
              {value.map((item, index) => (
                  <li key={index}>
                      {item.name}: {item.value} 
                  </li>

              ))}

          </ul>
      </div>
  )
}
