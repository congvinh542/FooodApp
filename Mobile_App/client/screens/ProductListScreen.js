import React, { useEffect, useState } from 'react';
import { FlatList, Text, TouchableOpacity, View } from 'react-native';
import * as Icon from 'react-native-feather';
import { useDispatch, useSelector } from 'react-redux';
import ProductRow from '../components/ProductRow';
import BasketIcon from '../components/basketIcon';
import { addToBasket, removeFromBasket } from '../slices/basketSlice';
import { setResturant } from '../slices/resturantSlice';
import { themeColors } from '../theme';

export default function ProductListScreen({ route, navigation }) {
    const { categoryId, resturantData } = route.params;
    const [productList, setProductList] = useState([]);
    const [selectedCategoryData, setSelectedCategoryData] = useState({});
    const dispatch = useDispatch();
    const basketItems = useSelector((state) => state.basket);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(
                    `https://47d5-14-191-242-235.ngrok-free.app/api/Product/ProductByCategory?categoryId=${categoryId}`,
                    {
                        method: 'GET',
                    }
                );
                const data = await response.json();
                if (data && data.entity && Array.isArray(data.entity)) {
                    setProductList(data.entity);

                    if (data.entity.length > 0) {
                        const firstProduct = data.entity[0];
                        setSelectedCategoryData({
                            nameCategory: firstProduct.nameCategory,
                            pathImage: firstProduct.pathCategory,
                        });
                    }
                } else {
                    console.log('Invalid data format:', data);
                }
            } catch (error) {
                console.log('Error fetching products by category:', error);
            }
        };

        fetchData();
    }, [categoryId]);

    const handleIncrease = (id, name, price, nameCategory, pathImage, pathCategory, description, quantity) => {
        dispatch(addToBasket({ id, name, price, nameCategory, pathImage, pathCategory, description, quantity }));
    };

    const handleDecrease = (id) => {
        dispatch(removeFromBasket({ id }));
    };

    const renderProductItem = ({ item, index }) => (
        <ProductRow
            key={item.id}
            id={item.id}
            name={item.name}
            description={item.nameCategory}
            price={item.price}
            pathImage={item.pathImage}
            quantity={item.quantity}
            pathCategory={item.pathCategory}
            basketItems={basketItems}
            handleIncrease={handleIncrease}
            handleDecrease={handleDecrease}
            isLastItem={index === productList.length - 1}
        />
    );

    const goToBasketScreen = () => {
        dispatch(setResturant(resturantData));
        navigation.navigate('Basket');
    };

    return (
        <>
            <BasketIcon onPress={goToBasketScreen} />
            <View style={{ flex: 1 }}>
                <TouchableOpacity
                    onPress={() => navigation.goBack()}
                    style={{
                        position: 'absolute',
                        top: 16,
                        left: 16,
                        backgroundColor: 'rgba(255, 255, 255, 0.8)',
                        padding: 12,
                        borderRadius: 30,
                        zIndex: 1,
                    }}
                >
                    <Icon.ArrowLeft strokeWidth={3} stroke={themeColors.bgColor(1)} />
                </TouchableOpacity>
                <View
                    style={{
                        backgroundColor: 'white',
                        marginTop: -12,
                        padding: 16,
                        paddingHorizontal: 20,
                        shadowColor: '#000',
                        shadowOffset: {
                            width: 0,
                            height: 2,
                        },
                        shadowOpacity: 0.25,
                        shadowRadius: 3.84,
                        elevation: 5,
                    }}
                >
                    <Text style={{ fontSize: 24, fontWeight: 'bold', color: 'black', textAlign: 'center', paddingTop: 20 }}>{selectedCategoryData.nameCategory}</Text>
                </View>
                <FlatList
                    data={productList}
                    style={{ backgroundColor: 'white' }}
                    keyExtractor={(item) => item.id.toString()}
                    renderItem={renderProductItem}
                />
            </View>
        </>
    );
}
