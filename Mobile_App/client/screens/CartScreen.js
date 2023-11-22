import { useNavigation } from '@react-navigation/native';
import React, { useEffect, useState } from 'react';
import { Image, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import * as Icon from 'react-native-feather';
import { useDispatch, useSelector } from 'react-redux';
import { removeFromBasket, selectBasketItems, selectBasketTotal } from '../slices/basketSlice';
import { themeColors } from '../theme';

export default function BasketScreen() {
    const basketItems = useSelector(selectBasketItems);
    const basketTotal = useSelector(selectBasketTotal);
    const [isPlacingOrder, setIsPlacingOrder] = useState(false);

    const dispatch = useDispatch();
    const navigation = useNavigation();
    const deliveryFee = 'Miễn phí';

    const placeOrder = async () => {
        try {
            // Kiểm tra xem giỏ hàng có sản phẩm không
            if (basketItems.length === 0) {
                alert('Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm để đặt hàng.');
                return;
            }

            // Tạo danh sách sản phẩm để gửi lên API
            const orderItems = basketItems.map(item => ({
                id: item.id,
                quantity: item.quantity,
                name: item.name,
                pathImage: item.pathImage,
                price: item.price,
            }));




            const totalAmountProduct = basketTotal.toString().slice(0, -1);

            // Tạo đối tượng dữ liệu đặt hàng
            const orderData = {
                name: orderItems[0]?.name,
                pathImage: orderItems[0]?.pathImage,
                totalAmount: totalAmountProduct, // Bạn cần thay thế 0 bằng tổng giá trị thực tế nếu có
                quantity: orderItems.length.toString(), // Số lượng sản phẩm, bạn có thể sửa đổi tùy thuộc vào cách bạn muốn xử lý
                // items: orderItems,
            };



            // Bắt đầu quá trình đặt hàng
            setIsPlacingOrder(true);

            // Gửi dữ liệu đặt hàng qua API
            const response = await fetch('https://47d5-14-191-242-235.ngrok-free.app/api/Order/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(orderData),
            });
            console.log('orderData:', orderData);
            // Kiểm tra kết quả từ API
            if (response.ok) {
                // alert('Đặt hàng thành công!');
                // dispatch(clearBasket());
                // Chuyển đến màn hình khác nếu cần
                navigation.navigate('PreparingOrder');
            } else {
                alert('Đặt hàng thất bại. Vui lòng thử lại sau.');
            }
        } catch (error) {
            console.error('Error placing order:', error);
            alert('Đã xảy ra lỗi khi đặt hàng. Vui lòng thử lại sau.');
        } finally {
            // Kết thúc quá trình đặt hàng
            setIsPlacingOrder(false);
        }
    };


    const [groupedItems, setGroupedItems] = useState([]);

    useEffect(() => {
        const gItems = basketItems.reduce((group, item) => {
            if (group[item.id]) {
                group[item.id].push(item);
            } else {
                group[item.id] = [item];
            }
            return group;
        }, {});
        setGroupedItems(gItems);
    }, [basketItems]);

    const handleRemoveFromBasket = (id) => {
        dispatch(removeFromBasket({ id }));
    };
    const formatCurrency = (amount) => {
        const formattedAmount = amount.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
        return formattedAmount.endsWith('.00') ? formattedAmount.slice(0, -3) : formattedAmount;
    };
    return (
        <View className="bg-white flex-1">
            {/* top button */}
            <View className="relative py-4 shadow-sm">
                <TouchableOpacity
                    style={{ backgroundColor: themeColors.bgColor(1) }}
                    onPress={navigation.goBack}
                    className="absolute z-10 rounded-full p-1 shadow top-5 left-2"
                >
                    <Icon.ArrowLeft strokeWidth={3} stroke="white" />
                </TouchableOpacity>
                <View>
                    <Text className="text-center font-bold text-xl">Giỏ hàng</Text>
                </View>
            </View>

            {/* delivery time */}
            <View style={{ backgroundColor: themeColors.bgColor(0.2) }} className="flex-row px-4 items-center">
                <Image source={require('../assets/images/bikeGuy.png')} className="w-20 h-20 rounded-full" />
                <Text className="flex-1 pl-4">Giao hàng sau 20-30 phút</Text>
                <TouchableOpacity>
                    <Text style={{ color: themeColors.text }} className="font-bold">
                        Thay đổi
                    </Text>
                </TouchableOpacity>
            </View>

            {/* dishes */}
            <ScrollView
                showsVerticalScrollIndicator={false}
                className="bg-white pt-5"
                contentContainerStyle={{
                    paddingBottom: 50,
                }}
            >
                {Object.entries(groupedItems).map(([key, items]) => (
                    <View
                        key={key}
                        style={{
                            flexDirection: 'row',
                            alignItems: 'center',
                            paddingHorizontal: 16,
                            paddingVertical: 8,
                            backgroundColor: 'white',
                            borderRadius: 16,
                            marginHorizontal: 16,
                            marginBottom: 8,
                            shadowColor: '#000',
                            shadowOffset: { width: 0, height: 2 },
                            shadowOpacity: 0.25,
                            shadowRadius: 3.84,
                            elevation: 5,
                        }}
                        className="flex-row items-center space-x-3 py-2 px-4 bg-white rounded-3xl mx-2 mb-3 shadow-md"
                    >
                        <Text style={{ color: themeColors.text }} className="font-bold">
                            {items.length} x{' '}
                        </Text>
                        <Image
                            className="h-20 w-20 rounded-lg"
                            source={{ uri: items[0]?.pathImage }} // Thay đổi từ 'image' sang 'pathImage'
                        />
                        <Text className="flex-1 font-bold text-gray-700">{items[0]?.name}</Text>
                        <Text className="font-semibold text-base">{formatCurrency(items[0]?.price * items.length)}đ</Text>
                        <TouchableOpacity
                            className="p-1 rounded-full"
                            style={{ backgroundColor: themeColors.bgColor(1) }}
                            onPress={() => handleRemoveFromBasket(items[0]?.id)}
                        >
                            <Icon.Minus strokeWidth={2} height={20} width={20} stroke="white" />
                        </TouchableOpacity>
                    </View>
                ))}
            </ScrollView>

            {/* totals */}
            <View style={{ backgroundColor: themeColors.bgColor(0.2) }} className=" p-6 px-8 rounded-t-3xl space-y-4">
                <View className="flex-row justify-between">
                    <Text className="text-gray-700">Tổng thu</Text>
                    <Text className="text-gray-700">{basketTotal}</Text>
                </View>
                <View className="flex-row justify-between">
                    <Text className="text-gray-700">Phí giao hàng</Text>
                    <Text className="text-gray-700">{deliveryFee}</Text>
                </View>
                <View className="flex-row justify-between">
                    <Text className="font-extrabold">Tổng số đơn hàng</Text>
                    <Text className="font-extrabold">{basketTotal}</Text>
                </View>
                <View>
                    <TouchableOpacity
                        style={{ backgroundColor: themeColors.bgColor(1) }}
                        onPress={placeOrder}
                        className="p-3 rounded-full"
                        disabled={isPlacingOrder}
                    >
                        <Text className="text-white text-center font-bold text-lg">
                            {isPlacingOrder ? 'Đang đặt hàng...' : 'Đặt hàng'}
                        </Text>
                    </TouchableOpacity>
                </View>
            </View>
        </View>
    );
    
}
