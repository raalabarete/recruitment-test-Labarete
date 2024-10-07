import React, { Component } from 'react';
import EmployeeList from './components/EmployeeList';
import ABCEmployeeList from './components/ABCEmployeeList';
export default class App extends Component {
  
    constructor(props) {
        super(props);
        this.state = {
            data: null,      
            isLoading: true,
            error: null,
        };
    }

    async fetchData() {
        fetch('http://localhost:41478/Employees')
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then((data) => {
                this.setState({
                    data: data,
                    isLoading: false,
                });
            })
            .catch((error) => {
                this.setState({
                    error: error,
                    isLoading: false,
                });
            });
    }

    componentDidMount() {
        this.fetchData();
    }
    
    render() {
        const { data, isLoading, error } = this.state;
        
        if (isLoading) {
            return <div>Loading...</div>;
        }

        if (error) {
            return <div>Error: {error.message}</div>;
        }

        if (!isLoading) {
            return (
                <div>
                    <div style={{ float: 'left', width: '50%' }}><EmployeeList employeeData={data}></EmployeeList></div>
                    <div style={{ float: 'left', width: '50%' }} ><ABCEmployeeList></ABCEmployeeList></div>
                </div>
            );
        }
  }
}
