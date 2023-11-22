import { format } from 'date-fns';
import React, { useEffect, useState } from 'react';
import { FlatList, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import { Avatar, Card, Divider } from 'react-native-elements';

const OrderListScreen = ({ route }) => {
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await fetch(
                    `https://47d5-14-191-242-235.ngrok-free.app/api/Order/GetOrderByUserId/A21D05FD-801A-4AA6-526D-08DBEB372DB8`
                );
                const data = await response.json();
                setOrders(data.entity);
            } catch (error) {
                console.error('Error fetching orders:', error);
            }
        };

        fetchOrders();
    }, []);


    const renderItem = ({ item }) => (
        <TouchableOpacity>
            <Card containerStyle={styles.card}>
                <View style={styles.cardContent}>
                    <Avatar
                        size='large'
                        rounded
                        source={{ uri: item.pathImage }} 
                        containerStyle={styles.avatarContainer}
                    />
                    <View style={styles.textContainer}>
                        <Text style={styles.orderIdText}>Mã đơn hàng: {item.code}</Text>
                        <Text>Tên sản phẩm: {item.name}</Text>
                        <Text>Số lượng: {item.quantity}</Text>
                        <Text>Tổng cộng: {item.totalAmount}</Text>
                        <Text>Ngày đặt hàng: {format(new Date(item.orderDate), 'HH:mm - dd/MM/yyyy')}</Text>
                    </View>
                </View>
                <Divider style={styles.divider} />
                <View style={styles.totalContainer}>
                    <Text style={styles.totalText}>Tổng tiền đơn hàng: {formatCurrency(item.totalAmount)} VNĐ</Text>

                </View>
            </Card>
        </TouchableOpacity>
    );
    const formatCurrency = (amount) => {
        const formattedAmount = amount.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
        return formattedAmount.endsWith('.00') ? formattedAmount.slice(0, -3) : formattedAmount;
    };
    return (
        <View style={styles.container}>
            <Text style={styles.headerText}>Danh sách đơn hàng</Text>
            <FlatList
                data={orders}
                keyExtractor={(item, index) => index.toString()}
                renderItem={renderItem}
                contentContainerStyle={styles.flatListContainer}
            />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#f5f5f5',
        padding: 16,
    },
    headerText: {
        fontSize: 24,
        fontWeight: 'bold',
        marginBottom: 16,
    },
    flatListContainer: {
        paddingBottom: 20,
    },
    card: {
        borderRadius: 10,
        padding: 0,
        marginBottom: 16,
    },
    cardContent: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: 16,
    },
    avatarContainer: {
        marginRight: 16,
    },
    textContainer: {
        flex: 1,
    },
    orderIdText: {
        fontSize: 18,
        fontWeight: 'bold',
        marginBottom: 4,
    },
    divider: {
        height: 1,
        backgroundColor: '#e0e0e0',
    },
    totalContainer: {
        flexDirection: 'row',
        justifyContent: 'flex-end',
        alignItems: 'center',
        padding: 16,
    },
    totalText: {
        fontSize: 16,
        fontWeight: 'bold',
    },
});

export default OrderListScreen;
