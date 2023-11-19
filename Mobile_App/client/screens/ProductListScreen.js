import React, { useEffect, useState } from 'react';
import { FlatList, Text, View } from 'react-native';
import { useDispatch, useSelector } from 'react-redux';
import ProductRow from '../components/ProductRow';
import BasketIcon from '../components/basketIcon';
import { addToBasket, removeFromBasket } from '../slices/basketSlice';

export default function ProductListScreen({ route }) {
    const { categoryId } = route.params;
    const [productList, setProductList] = useState([]);
    const dispatch = useDispatch();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(
                    `https://7f72-14-191-242-235.ngrok-free.app/api/Product/ProductByCategory?categoryId=${categoryId}`,
                    {
                        method: 'GET',
                    }
                );
                const data = await response.json();
                if (data && data.entity && Array.isArray(data.entity)) {
                    setProductList(data.entity);
                } else {
                    console.log('Invalid data format:', data);
                }
            } catch (error) {
                console.log('Error fetching products by category:', error);
            }
        };

        fetchData();
    }, [categoryId]);

    const basketItems = useSelector((state) => state.basket); // Lưu ý sửa dòng này để lấy dữ liệu từ Redux state

    const handleIncrease = (id, name, price, nameCategory, pathImage, description) => {
        dispatch(addToBasket({ id, name, price, nameCategory, pathImage, description }));
    };

    const handleDecrease = (id) => {
        dispatch(removeFromBasket({ id }));
    };

    const renderProductItem = ({ item }) => (
        <ProductRow
            key={item.id}
            id={item.id}
            name={item.name}
            description={item.nameCategory}
            price={item.price}
            pathImage={item.pathImage}
            basketItems={basketItems}
            handleIncrease={handleIncrease}
            handleDecrease={handleDecrease}
            
        />
    );

    return (
        <>    
        <BasketIcon />
        <View>
            <Text style={{ fontSize: 24, fontWeight: 'bold', margin: 16 }}>Danh sách sản phẩm</Text>
            <FlatList data={productList} keyExtractor={(item) => item.id.toString()} renderItem={renderProductItem} />
        </View>
        </>
    );
}
